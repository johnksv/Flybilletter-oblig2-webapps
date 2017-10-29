"LuftKlar" er en webapplikasjon for å bestille flybilletter.

Funksjonalitet knyttet til prosjektoppgave 2 er inkrementer på applikasjonen fra første prosjektoppgave. I navigasjonsbaren/menybaren vil knappen "Logg inn" vises dersom man ikke er innlogget som administrator. Hvis logget inn erstattes "Logg inn" knappen med "Administrer" som viser alle sider relatert til administrasjon av applikasjon. Utforming og styling av view-ene er gjort minimalistisk med vilje.

Rammeverk vi har benyttet oss av:
    - EntityFramework v6.1.3 (Microsoft)
    - jQuery v3.2.1 (jQuery Foundation)
    - jQuery Validate v1.16.0 (Jörn Zaeffer)
    - bootstrap v3.3.7 (Twitter, inc.)
    - Z.EntityFramework.Extensions v3.13.5 (ZZZ Projects Inc.)
    - Automapper v6.1.1 (Jimmy Bogard)
    - List.js v1.5.0 (Jonny Strömberg)
    - StudioDonder.MvcContrib.Mvc4.TestHelper v3.0.0.99 (Outercurve Foundation, Erik Schierboom)

Andre data vi har benyttes oss av:
    - Postnummer i Norge - Publisert 27.02.2015 (https://data.norge.no/data/posten-norge/postnummer-i-norge)

Anmerkelser fra prosjektoppgave 2:
    - Flygninger genereres i DAL/DBInit.cs, og det genereres flygninger fra dags dato (29.10.2017) til og med to uker frem i tid.
    - Deler av applikasjon som DAL er ikke natulig å teste med enhetstester (burde gjøres som integrasjonstester), og har derfor blitt ekskludert fra Code Coverage. Andre deler som BLL blir implisitt testet gjennom AdminController. Deler av BLL tilknyttet prosjektoppgave 1 er ikke testet, og dette trekker ned vår Code Coverage prosentandel.
    - Administrator kan ikke slette elementer fra entitetene "Flyplass" og "Flygning" (Flygning sin "kanseler"-operasjon oppdaterer en status). Disse begrensningene er gjort bevisst.
    - Endringer og feilmeldinger blir ikke populert ved programmets oppstart. Derfor kan tabellene i Administrator-viewet være uten innhold frem til en endring eller feil har skjedd.

Anmerkelser fra prosjektoppgave 1:
    - Webapplikasjonen tar kun høyde for reiser med 0 eller 1 mellomlanding. Støtte for flere mellomlandinger kan løses ved å bruke rammeverket "QuickGraph". Dette ville blitt implementert som følger: Flyplasser angis som noder, og et graf-tre bygges rundt dette. Alle kanter mellom nodene representerer flyruter, og man angir en vekt på disse rutene. I vårt tilfelle ville denne vekten enten vært reisetid eller distanse. Basert på vår tolkning av oppgaveteksten, valgte vi å ikke implementere dette.
    
Link til webapplikasjonen:
    http://flybilletterwebapps2017.azurewebsites.net/

Påloggingsinformasjon for administrator:
    Brukernavn: "root"
    Passord: "Test1"
