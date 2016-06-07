import java.nio.file.Paths;

public class EnvironmentVariables {
    public void setDataDirectory(String _dataDirectory) {
        dataDirectory = _dataDirectory;
        novelsDirectory = Paths.get(dataDirectory, "Novels").toString();
        configurationFile = Paths.get(dataDirectory, "Config.xml").toString();
    }

    public static final String NOVEL_EXTENSION = "vnxml",
            PACKED_NOVEL_EXTENSION = "vnzip";
    static String dataDirectory = Paths.get("").toAbsolutePath().toString();
    static String novelsDirectory = Paths.get(dataDirectory, "Novels").toString();
    static String configurationFile = Paths.get(dataDirectory, "Config.xml").toString();
}