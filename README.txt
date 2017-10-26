"LuftKlar" er en webapplikasjon for å bestille flybilletter.

Funksjonalitet knyttet til prosjektoppgave 2 er inkrementer på applikasjonen fra første prosjektoppgave. I navigasjonsbaren/menybaren vil knappen "Logg inn" vises dersom man ikke er innlogget som administrator. Hvis logget inn erstattes "Logg inn" knappen med "Administrer" som viser alle "admin-sider" for vedlikehold av database-entiteter. Utforming og styling av view-ene er gjort minimalistisk med vilje.

Rammeverk vi har benyttet oss av:
    - EntityFramework v6.1.3 (Microsoft)
    - jQuery v3.2.1 (jQuery Foundation)
    - jQuery Validate v1.16.0 (Jörn Zaeffer)
    - bootstrap v3.3.7 (Twitter, inc.)
    - Z.EntityFramework.Extensions v3.13.5 (ZZZ Projects Inc.)
    - Automapper v6.1.1 (Jimmy Bogard)
    - List.js v 1.5.0 (Jonny Strömberg)

Andre data vi har benyttes oss av:
	- Postnummer i Norge - Publisert 27.02.2015 (https://data.norge.no/data/posten-norge/postnummer-i-norge)

Anmerkelser fra prosjektoppgave 2:
    - Flygninger genereres i DAL/DBInit.cs, og det genereres flygninger fra dags dato til og med tre uker frem i tid.

Anmerkelser fra prosjektoppgave 1:
    - Webapplikasjonen tar kun høyde for reiser med 0 eller 1 mellomlanding. Støtte for flere mellomlandinger kan løses ved å bruke rammeverket "QuickGraph". Dette ville blitt implementert som følger: Flyplasser angis som noder, og et graf-tre bygges rundt dette. Alle kanter mellom nodene representerer flyruter, og man angir en vekt på disse rutene. I vårt tilfelle ville denne vekten enten vært reisetid eller distanse. Basert på vår tolkning av oppgaveteksten, valgte vi å ikke implementere dette.
    
Link til webapplikasjonen:
    http://flybilletterwebapps2017.azurewebsites.net/

