"LuftKlar" er en webapplikasjon for å bestille flybilletter.
Rammeverk vi har benyttet oss av:
    - EntityFramework v6.1.3 (Microsoft)
    - jQuery v3.2.1 (jQuery Foundation)
    - jQuery Validate v1.16.0 (Jörn Zaeffer)
    - bootstrap v3.3.7 (Twitter, inc.)

Anmerkelser:
    - Webapplikasjonen tar kun høyde for reiser med 0 eller 1 mellomlanding. Støtte for flere mellomlandinger kan løses ved å bruke rammeverket "QuickGraph". Dette ville blitt implementert som følger: Flyplasser angis som noder, og et graf-tre bygges rundt dette. Alle kanter mellom nodene representerer flyruter, og man angir en vekt på disse rutene. I vårt tilfelle ville denne vekten enten vært reisetid eller distanse. Basert på vår tolkning av oppgaveteksten, valgte vi å ikke implementere dette.
    
Link til webapplikasjonen:
    http://flybilletterwebapps2017.azurewebsites.net/

