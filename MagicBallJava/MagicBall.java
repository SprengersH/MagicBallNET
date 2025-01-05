import java.io.*;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Map;

import gnu.io.*;

public class MagicBall implements AutoCloseable {
    private final SerialPort serialPort;
    private final InputStream inputStream;
    private final OutputStream outputStream;
    private final Charset encoding = Charset.forName("IBM437");
    private final int interCharPauseMs = 50;

    public MagicBall(String portName) throws Exception {
        CommPortIdentifier portIdentifier = CommPortIdentifier.getPortIdentifier(portName);
        if (portIdentifier.isCurrentlyOwned()) {
            throw new IOException("Port is currently in use");
        }

        CommPort commPort = portIdentifier.open("MagicBall", 2000);
        if (commPort instanceof SerialPort) {
            serialPort = (SerialPort) commPort;
            serialPort.setSerialPortParams(4800, SerialPort.DATABITS_8, SerialPort.STOPBITS_1, SerialPort.PARITY_EVEN);
            inputStream = serialPort.getInputStream();
            outputStream = serialPort.getOutputStream();
        } else {
            throw new IOException("Not a serial port");
        }
    }

    @Override
    public void close() throws Exception {
        if (serialPort != null) {
            serialPort.close();
        }
    }

    public Map<String, String> getDeviceInfo() throws IOException {
        writeCommand(new byte[]{0x1B, 0x53, 0x03});
        byte[] rawData = readUntil((byte) 0x03);
        String[] parts = new String(rawData, encoding).split("\0");

        Map<String, String> info = new HashMap<>();
        if (parts.length > 0) info.put("Versie", parts[0]);
        if (parts.length > 1) info.put("Fabrikant", parts[1]);
        if (parts.length > 6) info.put("Serienummer", parts[6]);
        if (parts.length > 8) info.put("Standaard Tekst", parts[7] + " / " + parts[8]);
        if (parts.length > 10) info.put("Font", parts[9] + " / " + parts[10]);
        if (parts.length > 11) info.put("Geheugen", parts[11]);

        return info;
    }

    public void sendFormattedText(String text, int extraWhitespace) throws IOException {
        if (text == null || text.isEmpty()) {
            throw new IllegalArgumentException("Tekst mag niet leeg zijn.");
        }

        String paddedText = text + " ".repeat(extraWhitespace);
        writeCommand(new byte[]{0x02, 0x0D});
        writeData(paddedText.getBytes(encoding));
        writeCommand(new byte[]{0x03});
    }

    private void writeCommand(byte[] command) throws IOException {
        for (byte b : command) {
            outputStream.write(b);
            try {
                Thread.sleep(interCharPauseMs);
            } catch (InterruptedException ignored) {
            }
        }
    }

    private void writeData(byte[] data) throws IOException {
        for (byte b : data) {
            outputStream.write(b);
            try {
                Thread.sleep(interCharPauseMs);
            } catch (InterruptedException ignored) {
            }
        }
    }

    private byte[] readUntil(byte lastByte) throws IOException {
        ByteArrayOutputStream buffer = new ByteArrayOutputStream();
        int b;
        while ((b = inputStream.read()) != -1) {
            buffer.write(b);
            if (b == lastByte) break;
        }
        return buffer.toByteArray();
    }
}
