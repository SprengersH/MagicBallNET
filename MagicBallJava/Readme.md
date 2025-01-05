# Lichtkrant CLI

Dit project heb ik ontwikkeld om een lichtkrant aan te sturen via een seriële verbinding. Het ondersteunt zowel C# als Java en biedt functionaliteit zoals apparaat-informatie ophalen, tekst versturen en geheugen onderzoeken.

## Hoe ik dit heb gerealiseerd

### Stap 1: Basisconfiguratie
Ik ben begonnen met het configureren van de seriële verbinding. Door te experimenteren en verschillende instellingen uit te proberen, kwam ik uiteindelijk tot de juiste configuratie:
- **Baudrate**: 4800
- **Parity**: Even
- **Data bits**: 8
- **Stop bits**: 1

### Stap 2: Onderzoek en implementatie
Ik heb kleine programma's geschreven om te testen hoe het apparaat reageerde. Eerst stuurde ik eenvoudige leescommando's om te controleren of er een reactie kwam. Het apparaat retourneerde gestructureerde gegevens, die ik stap voor stap decodeerde. Zo kreeg ik een beter begrip van het protocol en de werking.

### Stap 3: Tekst versturen
Na het begrijpen van de basisfunctionaliteit, heb ik functies toegevoegd om tekst te versturen naar de lichtkrant. Het apparaat accepteert tekst in **IBM437-encoding**. Om ervoor te zorgen dat de tekst goed op het scherm wordt weergegeven, heb ik extra spaties toegevoegd aan de tekst, zodat deze niet te snel herhaalt.

### Java vs. C#
Ik heb zowel in C# als in Java dezelfde functionaliteit geïmplementeerd. In Java maak ik gebruik van de **RXTX-bibliotheek** voor seriële communicatie, terwijl ik in C# gebruik maak van **System.IO.Ports**. Beide versies zijn functioneel gelijk.

## Hoe te gebruiken
1. Sluit de lichtkrant aan op een beschikbare COM-poort.
2. Pas de COM-poort in de code aan zodat deze overeenkomt met de poort waaraan de lichtkrant is verbonden.
3. Compileer en voer het programma uit in de taal die je wilt gebruiken.
4. Kies een optie in de CLI en volg de instructies.

## Toekomstige uitbreidingen
- Geavanceerd geheugenonderzoek om meer inzicht te krijgen in de werking van het apparaat.
- Een GUI bouwen om het gebruik makkelijker te maken.
- Verbeterde foutafhandeling en logging voor robuustere prestaties.

---

Met dit project heb ik stap voor stap de lichtkrant onder controle gekregen en heb ik geleerd hoe ik de seriële communicatie effectief kan toepassen.
