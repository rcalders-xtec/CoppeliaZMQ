# CoppeliaZMQ

Codi font del programa d'interfície entre CoppeliaSim i Codesys. Interfície Utilitzada en els cursos 23-24IAPG i 23-24PAPG. Programat amb Visual Studio 2022. 

A l'arxiu OPC.cs dins de la carpeta Llibreria hi ha l'adreça OPC dels nodes utilitzats per a les entrades i sortides, actualment per a Codesys. Es pot canviar per poder funcionar amb altres entorns de porgramació de PLCs:
```
            // Nodes de les variables de sortida i entrada
            string nodeIdToSubscribe = "ns=4;s=|var|CODESYS Control Win V3 x64.Application.Sinc.Sortides";
            string nodeIdToWrite = "ns=4;s=|var|CODESYS Control Win V3 x64.Application.Sinc.Entrades";
            //
```
