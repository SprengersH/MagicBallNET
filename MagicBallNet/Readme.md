# Lichtkrant CLI

Dit project is ontwikkeld om een lichtkrant aan te sturen via een seriële verbinding. Het biedt functionaliteit zoals het ophalen van apparaat-informatie, het versturen van tekst en het onderzoeken van het geheugen. Zowel implementaties in C# als Java zijn beschikbaar.

---

## Hoe dit project tot stand is gekomen

### Stap 1: Basisconfiguratie
Om de lichtkrant te kunnen aansturen, werd eerst de seriële verbinding geconfigureerd. Na experimenteren met verschillende instellingen bleek de juiste configuratie te zijn:
- **Baudrate**: 4800
- **Parity**: Even
- **Data bits**: 8
- **Stop bits**: 1

### Stap 2: Onderzoek en implementatie
Eerst zijn kleine programma's geschreven om de reacties van het apparaat te testen. Simpele leescommando's werden verzonden om te controleren of er een respons kwam. De ontvangen gegevens zijn vervolgens stap voor stap geanalyseerd en geïnterpreteerd.

### Stap 3: Tekst versturen
De lichtkrant accepteert tekst in IBM437-encoding. Een functie werd toegevoegd om teksten te verzenden, waarbij optioneel extra spaties kunnen worden ingevoegd om de weergave op het apparaat aan te passen.

---

## Geheugenonderzoek

### Functionaliteit
Het project bevat meerdere opties om het geheugen van de lichtkrant te onderzoeken:

1. **Geheugenpagina's onderzoeken**  
   Hiermee wordt de inhoud van specifieke geheugenpagina's opgehaald. Dit kan helpen om inzicht te krijgen in de structuur van het geheugen.

2. **Geoptimaliseerd geheugenonderzoek**  
   Deze functie doorzoekt het geheugen op opeenvolgende adressen. Elk adres wordt getest op relevante gegevens, wat helpt bij het identificeren van bruikbare geheugenlocaties.

3. **Geheugenkaart bekijken**  
   Deze optie geeft een overzicht van het volledige geheugen, waarbij per geheugenadres de inhoud wordt weergegeven.

4. **Zoeken in geheugen**  
   Hiermee wordt een zoekterm in het geheugen gezocht. Het resultaat toont waar in het geheugen de opgegeven tekst voorkomt.

---

## Status van het project

Hoewel er al veel functionaliteit beschikbaar is, zijn er nog open vragen over het geheugen en de interne structuur van de lichtkrant. Om meer duidelijkheid te krijgen, is contact opgenomen met de fabrikant van het apparaat. Hun feedback kan verdere verbeteringen en uitbreidingen mogelijk maken.

---

## Hoe te gebruiken

1. Sluit de lichtkrant aan op een beschikbare COM-poort.
2. Pas in de code de COM-poortinstelling aan naar de juiste waarde.
3. Compileer en voer het programma uit.
4. Kies een optie in de CLI om interactie te hebben met de lichtkrant.

---

## Toekomstige uitbreidingen
- Meer inzicht in het geheugen na feedback van de fabrikant.
- Verbeterde zoekfunctionaliteit binnen het geheugen.
- Eventuele GUI-ondersteuning om de lichtkrant nog toegankelijker te maken.
- Betere foutafhandeling en logging.

