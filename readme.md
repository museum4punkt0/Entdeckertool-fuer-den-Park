# **Inhaltsverzeichnis**

## **[Kurzbeschreibung](#Kurzbeschreibung)**

Bäume, Sträucher, Wiese – ansonsten nichts! Oder doch? Ein Klick und aus deinem Handy wird ein Entdeckertool mit allem was du brauchst, um Funde zu entdecken und herauszufinden was hier vor 2000 Jahren geschah. Denn 9 n. Chr. ereignete sich hier eine der berühmtesten römischen Schlachten – die Varusschlacht. Komm mit und mache dich mit der VAP-App im Park auf die Suche nach der Archäologie der Varusschlacht.
Wo wurde die Maske entdeckt? Was hat man sonst noch gefunden? Wo wurde schon gegraben? Geh einfach los oder wähle eine Tour. Und dann lass dich überraschen. Entdecke Funde, triff Experten, erwecke den Legionär zum Leben und bringe die Funde zum Schweben. Wie steht dir eigentlich die Maske und wo lebten damals die Germanen. Jede Menge Wissen, sechs Touren, 12 Spiele, viele Überraschungen – da ist für jeden was dabei.

Ziel der App ist es, ein Vermittlungstool zu bieten, dass den Nutzer:innen das selbstständige Erkunden des 10 Hektar großen Museumsparks ermöglicht. Seit mehr als 30 Jahren finden im Museumspark archäologische Ausgrabungen zur Varusschlacht statt, deren Ergebnisse, Befunde und Funde in der Dauerausstellung präsentiert werden. In der Landschaft selbst – dem historischen Tatort – ist hievon nach Abschluss der Ausgrabungen allerdings nichts mehr zu sehen. Anstelle der Installation unzähliger Informationstafeln wurde deshalb eine App entwickelt, die es den Besucher:innen ermöglichen soll die Forschung im Raum anhand ausgewählter Befundkontexte nachzuvollziehen und hierbei auch Fundkontexte eigenständig zu entdecken.

Die App navigiert die Besucher:innen anhand GPS-verbundener Points of Interest (PoI) zu den Fundstellen Verknüpft mit den PoI sind digitale Kontexträume, die sich bei Annäherung freischalten und Informationen in Form von Bildern, Texten und Videos auf verschiedenen Vertiefungsebenen zu den einzelnen Funden zur Verfügung stellen. Ergänzend dazu bietet die App unterschiedliche Spiele (teilweise als AR-Spiele) und Thementouren mit ausgewählten PoI an, die spezifische methodische Forschungsaspekte in den Mittelpunkt rücken, angefangen bei der Prospektion über die Ausgrabung hin zur Restaurierung. Einige erlebnisorientierte Überraschungsmomente runden das Aktiv-Angebot ab. Sehr Interessierten bietet sich überdies die Möglichkeit, mit Hilfe der App im Gelände Entwicklung und Fortgang Forschungsansatzes nachzuverfolgen und den Zusammenhang zwischen Fragestellung und Grabungsdesign nachzuvollziehen.

## **[Förderhinweis](#Förderhinweis)**

Die VAP-App, unser interaktives Tool zur Varusschlacht + Archäologie im Park ist entstanden Verbundprojekt „museum4punkt0 – Digitale Strategien für das Museum der Zukunft“, Teilprojekt Tracking the Past – Vom Forschungsfeld zum Erlebnisraum, Varusschlacht im Osnabrücker Land Museum und Park Kalkriese.
Das Projekt museum4punkt0 wird gefördert durch die Beauftragte der Bundesregierung für Kultur und Medien aufgrund eines Beschlusses des Deutschen Bundestages. Weitere Informationen: www.museum4punkt0.de

![alt text](https://github.com/museum4punkt0/media_storage/blob/2c46af6cb625a2560f39b01ecb8c4c360733811c/BKM_Fz_2017_Web_de.gif) ![alt text](https://github.com/museum4punkt0/media_storage/blob/e87f37973c3d91e2762d74d51bed81de5026e06e/BKM_Neustart_Kultur_Wortmarke_pos_RGB_RZ_web.jpg)

## **[Installation](#Installation)**

  ### Installation der Unity-Applikation
  
   - Voraussetzungen
      - Unity 2021.8f1
      - XCode 14

   - Unity-Plugins
      - Unity
        - 2D Sprite
        - AR Foundation
        - ARKit XR Plugin
        - ARKit Face Tracking
        - Burst
        - Mobile Notifications
        - ProBuilder
        - TextMeshPro
        - Timeline
        - Unity UI
        - Universal RP
      - Weitere Plugins
        - [AR + GPS Location](https://docs.unity-ar-gps-location.com/)
        - [AV Pro Video - Core Edition](https://renderheads.com/products/avpro-video/)
        - [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
        - [Lean Touch](https://assetstore.unity.com/packages/tools/input-management/lean-touch-30111)
    - Aufbau der Entwicklungsumgebung
      - Download 3D-Assets
        Das Projekt verwendet 3D-Assets, die nicht in diesem Repository enthalten sind. Diese müssen vor dem Build heruntergeladen werden. Dazu muss der Ordner `Assets/0_3dAssets` angelegt werden. In diesem Ordner müssen die folgenden Assets abgelegt werden: [https://var-production.xailabs.com/static/0_3dAssets.zip](https://vap-production.kalkriese-varusschlacht.de/static/0_3dAssets.zip)

      - Installation der Unity-Plugins
        Die Unity-Plugins müssen über den Unity Package Manager installiert werden. Dazu muss im Unity-Editor unter `Window > Package Manager` der Package Manager geöffnet werden. Dort können die Plugins über den Button `+` installiert werden.

      - Kompilieren der App
        Die App kann über den Unity-Editor kompiliert werden. Dazu muss im Unity-Editor unter `File > Build Settings` die Build Settings geöffnet werden. Dort kann die App über den Button `Build` kompiliert werden. Die App wird dann in dem angegebenen Ordner gespeichert.

      - Installation der App auf dem iPhone
        Die App kann über XCode auf dem iPhone installiert werden. Dazu muss das iPhone mit dem Computer verbunden werden. In XCode muss dann das iPhone als Zielgerät ausgewählt werden. Anschließend kann die App über den Button `Run` auf dem iPhone installiert werden.

      - Installation der App auf Android Geräten via adb
        Die App kann über adb auf Android Geräten installiert werden. Dazu muss das Android Gerät mit dem Computer verbunden werden. In der Kommandozeile muss dann der Befehl `adb install <path-to-apk>` ausgeführt werden. Der Pfad zur APK-Datei kann über die Build Settings im Unity-Editor ermittelt werden.

      - Hinweise zu Entwicklungs-Zertifikaten für iOS
        Die App kann nur auf einem iPhone installiert werden, wenn das iPhone für die Entwicklung registriert ist. Dazu muss das iPhone in der Apple Developer Console registriert werden. Anschließend muss ein Entwicklungs-Zertifikat für das iPhone erstellt werden. Dieses Zertifikat muss dann in XCode hinterlegt werden. Anschließend kann die App auf dem iPhone installiert werden.

  ### Installation des Strapi-Backends
   - Voraussetzungen
      - Node.js 18.12.1
      - npm 8.19.2
      - psql (PostgreSQL) 12.14
    - Installation
      Nach Installation der Voraussetzungen kann das Backend über die Kommandozeile installiert werden. Dazu muss in der Kommandozeile in den Ordner `backend` navigiert werden. Dort muss der Befehl `npm install` ausgeführt werden. Anschließend muss die Datei `.env.example` in `.env` umbenannt werden. In der Datei `.env` müssen die Zugangsdaten für die Datenbank eingetragen werden (DATABASE_HOST, DATABASE_PORT, DATABASE_USER,DATABASE_PASSWORD, DATABASE_SCHEMA). Anschließend kann das Backend über den Befehl `npm run develop` gestartet werden. Das Backend ist dann unter `http://localhost:1337` erreichbar.

# **[Benutzung](#Benutzung)**

  ## Benutzung & Weiterentwicklung Unity-App

  Die Unity-App basiert auf folgenden Frameworks:

  ### AR Foundation
  AR Foundation ist ein Framework, das es ermöglicht, AR-Anwendungen für verschiedene Plattformen zu entwickeln. AR Foundation bietet eine einheitliche API für AR-Anwendungen, die auf ARCore und ARKit basieren. 
  
  In "Varusschlacht" wird in folgenden Unity-Szenen AR Foundation verwendet:
  - Game 3 / Grabungscamp
      Game 3 ist ein mehrstufiges AR-Spiel in dem Spieler:innen unterschiedliche Aufgaben lösen müssen. Nachdem unterschiedliche POIs besucht wurden kann in einer AR-Szene ein Grabungscamp aufgebaut werden (Platzieren von Gegenständen im Raum, "Aufbau" eines Zeltes).
      Im zweiten Schritt des Spieles kann ein Grabungsschnitt freigelegt werden. Dazu müssen Spieler:innen in der AR-Szene Werkzeuge via Drag&Drop auf einen markierten Bereich in einer bestimmten Reihenfolge platzieren.
      
  - Game 5 / Legionär
      "Spiel 5" zeigt ein transparentes 3D-Modell eines Legionärs. Spieler:innen können den Legionär mit zuvor gefundenen Fundobjekten via drag&drop ausrüsten. Die Ausrüstung wird dabei an dem Legionär platziert.
      
  - ARFilterScene(Helmet) / Selfie mit Helm/Maske
      AR Foundation platziert in dieser Szene 2 3D-Modelle auf dem Kopf des Spielers. Zwischen den zwei Modellen kann gewechselt werden. Spieler:innen können Selfies mit den Modellen in deren Foto-Galerie speichern.
      
  - Game 7 / AR Reiter
      In dieser Szene wird ein 3D-Modell eines Reiters in der AR-Szene platziert und relativ zur Kamera bewegt.
      
  - 360_Illustrations
      Diese Szene zeigt einen "ARMedienContainer", also eine, im Backend konfigurierte, Beschreibung einer 3D Szene im AR Raum. In "ARMedienContainern" können 3D-Scans (müssen in der App vorhanden sein) oder Sprites (Bilder, welche im Backend hochgeladen werden) in einem AR Raum platziert werden. Detailierte Informationen zur Konfiguration von "ARMedienContainern" finden sich im Handbuch.
      
  ### Mapbox
   Mapbox dient als Rendering-Engine für die Karte. Mapbox bietet eine einheitliche API für die Darstellung von Karten auf verschiedenen Plattformen. Und ist somit ein Zentrales UI Element der App.

  ### Unity UI
   Neben den UI-Elementen des Menüs und der Spiele wird Unity UI verwendet, um die Inhalte der Parktouren sowie der Fundobjekte anzuzeigen.

  ## Benutzung & Weiterentwicklung Strapi-Backend
   siehe Handbuch

## [Credits](#Credits)

## [Lizenz](#Lizenz)
  - Die hier hinterlegte Anwendung wird unter der GNU GPLv3 Lizenz veröffentlicht. 
  - Sämtliche verwendete Programmteile Dritter sind mit der jeweiligen Lizenz:
    - Unity 
