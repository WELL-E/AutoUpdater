﻿using GeneralUpdate.Core.Events;
using GeneralUpdate.Core.Pipelines.Context;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Differential.Config;
using GeneralUpdate.Zip;
using GeneralUpdate.Zip.Factory;
using System;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Middleware
{
    public class ZipMiddleware : IMiddleware
    {
        public async Task InvokeAsync(BaseContext context, MiddlewareStack stack)
        {
            Exception exception = null;
            try
            {
                context.OnProgressEventAction(this, ProgressType.Updatefile, "In the unzipped file ...");
                var version = context.Version;
                bool isUnzip = UnZip(context);
                if (!isUnzip) throw exception = new Exception($"Unzip file failed , Version-{ version.Version }  MD5-{ version.MD5 } !");

                await ConfigFactory.Instance.Scan(context.SourcePath, context.TargetPath);
                var node = stack.Pop();
                if (node != null) await node.Next.Invoke(context, stack);
            }
            catch (Exception ex)
            {
                exception = exception ?? ex;
                context.OnExceptionEventAction(this, exception);
                throw exception;
            }
        }

        /// <summary>
        /// UnZip
        /// </summary>
        /// <param name="zipfilepath"></param>
        /// <param name="unzippath"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected bool UnZip(BaseContext context)
        {
            try
            {
                bool isComplated = false;
                var generalZipfactory = new GeneralZipFactory();
                generalZipfactory.UnZipProgress += (sender, e) => context.OnProgressEventAction(this, ProgressType.Updatefile, "Updatting file...");
                generalZipfactory.Completed += (sender, e) => isComplated = true;
                generalZipfactory.CreatefOperate(MatchType(context.Format), context.ZipfilePath, context.TargetPath, false, context.Encoding).
                    UnZip();
                return isComplated;
            }
            catch (Exception ex)
            {
                context.OnExceptionEventAction(this, ex);
                return false;
            }
        }

        private OperationType MatchType(string extensionName)
        {
            OperationType type = OperationType.None;
            switch (extensionName)
            {
                case ".zip":
                    type = OperationType.GZip;
                    break;

                case ".7z":
                    type = OperationType.G7z;
                    break;
            }
            return type;
        }
    }
}