import java.util.Map;

public class Main {
    public static void main(String[] args) {
        System.out.println("== Lichtkrant CLI ==");

        try (MagicBall magicBall = new MagicBall("COM6")) {
            Map<String, String> deviceInfo = magicBall.getDeviceInfo();
            System.out.println("Apparaat Informatie:");
            deviceInfo.forEach((key, value) -> System.out.println(key + ": " + value));

            System.out.print("Voer de tekst in die je wilt weergeven: ");
            String text = System.console().readLine();
            System.out.print("Voer het aantal extra spaties in (standaard: 10): ");
            int extraWhitespace = Integer.parseInt(System.console().readLine());

            magicBall.sendFormattedText(text, extraWhitespace);
            System.out.println("Tekst verstuurd!");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
