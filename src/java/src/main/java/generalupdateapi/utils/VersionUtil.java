package generalupdateapi.utils;

import java.security.MessageDigest;
import org.apache.commons.codec.binary.Hex;

public class VersionUtil {
    public static String getMD5(byte[] bytes)throws Exception{
        MessageDigest MD5 = MessageDigest.getInstance("MD5");
        MD5.update(bytes,0,bytes.length);
        return new String(Hex.encodeHex(MD5.digest()));
    }
}
