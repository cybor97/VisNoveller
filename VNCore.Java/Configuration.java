import com.sun.org.apache.xerces.internal.impl.PropertyManager;
import com.sun.org.apache.xerces.internal.impl.XMLStreamReaderImpl;
import com.sun.xml.internal.stream.writers.XMLStreamWriterImpl;

import javax.xml.stream.XMLStreamException;
import javax.xml.stream.XMLStreamReader;
import javax.xml.stream.XMLStreamWriter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;

public class Configuration {
    public static String userName;
    public static String language;
    public static Boolean fullscreen;
    public static Boolean autoSave;
    public static Integer soundVolume;

    public static void init() throws IOException, XMLStreamException {
        if (Files.exists(Paths.get(EnvironmentVariables.configurationFile)))
            load();
        else {
            fullscreen = true;
            save();
        }
    }

    public static void save() throws XMLStreamException, IOException {
        XMLStreamWriter writer = new XMLStreamWriterImpl(new FileOutputStream(EnvironmentVariables.configurationFile), "UTF-8", new PropertyManager(PropertyManager.CONTEXT_WRITER));
        writer.writeStartElement("Config");
        writer.writeAttribute("UserName", userName);
        writer.writeAttribute("Language", language);
        writer.writeAttribute("Fullscreen", fullscreen.toString());
        writer.writeAttribute("AutoSave", autoSave.toString());
        writer.writeAttribute("SoundVolume", soundVolume.toString());
        writer.writeEndElement();
    }

    public static void load() throws XMLStreamException, IOException {
        if (Files.exists(Paths.get(EnvironmentVariables.configurationFile))) {
            XMLStreamReader reader = new XMLStreamReaderImpl(new FileInputStream(EnvironmentVariables.configurationFile), "UTF-8", new PropertyManager(PropertyManager.CONTEXT_WRITER));
            while (reader.hasNext())
                if (reader.isStartElement() && reader.getLocalName().equals("Config")) {
                    String namespaceUri = reader.getNamespaceURI();
                    userName = reader.getAttributeValue(namespaceUri, "UserName");
                    language = reader.getAttributeValue(namespaceUri, "Language");
                    fullscreen = Boolean.parseBoolean(reader.getAttributeValue(namespaceUri, "Fullscreen"));
                    autoSave = Boolean.parseBoolean(reader.getAttributeValue(namespaceUri, "AutoSave"));
                    soundVolume = Integer.parseInt(reader.getAttributeValue(namespaceUri, "SoundVolume"));
                    reader.next();
                }
        }
    }

}