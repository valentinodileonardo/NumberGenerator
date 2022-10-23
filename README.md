# NumberGenerator
Bulk-Generation tool for international mobile phone numbers into csv for Google Contacts Import OR generate a simple .vcf file that contains all generated contacts.

This tools allows you to create thousands of mobile numbers using a specific format and save them to your contacts. 
This CSV can be uploaded to your Google Contacts cloud. A synchronization to your phone should be started automatically. This might take some minutes, depending on how many new contacts you've uploaded. 
Or if you generate a .vcf file you can just copy it to your phone and directly import it. I suggest to import them into a location you do not use, so it is
easier to remove them at the end when you are done.

If once of above is done, you should see all available WhatsApp contacts in your WhatsApp contacts list. 
Of course only phone numbers associated with a WhatsApp account are displayed.

What do you need?
- Google Contacts Mode
    - A smartphone which is logged in into your Google account 
    - [Google Contacts][GoCo] to upload the generated CSV file
- VCF Mode
    -A smartphone which is able to import vcf file
    - WhatsApp
    - .NET Framework 4.6.1 or higher
    - NumberGenerator


# NumberGenerator Usage
```
NumberGenerator 1.0.3.0
Copyright: GNU GPLv3

  -d, --digits    Required. Specifiy how many digits should be generated (exclusive prefix), e.g. 7
  -p, --prefix    Required. Specifiy the international prefix AND provider prefix, e.g. +49177
  -l, --label     Label (group) all generated contacts will be assigned to AND prefix of contact name default:
                  MyNewContacts
  -a, --amount    Amount of numbers you whish to generate, default: 10.000. Googles max is 12K
  -g, --google    Specify the mode g for google csv format, vcf default.
  --help          Display this help screen.
  --version       Display version information.
```



# Sample Execution [Google Contacts CSV]
Execution for German phone numbers
```
NumberGenerator.exe -a5000 -p+49177 -lGermany -d7 -g
```

Execution for Colombian phone numbers
```
NumberGenerator.exe -a5000 -p+57350 -lColombia -d7 -g
```

# Sample Execution [Plain VCF]
Execution for German phone numbers
```
NumberGenerator.exe -a5000 -p+49177 -lGermany -d7 
```

# Upload [Google Contacts Only]
To upload the CSV visit [Google Contacts][GoCo] and go to import, select the just created contacts.csv file and import it. 
Do not upload a file with more than 12.000 entries, as this is the Google limit. Your phone will start synchronize automatically and WhatsApp will add each new contact into it, if available.

# Disclaimer
This repository and all software it contains is for educational purposes only. Do not use it for illegal activities. You are solely responsible for your actions! I take NO responsibility and/or liability for how you choose to use any of the source code available here. By using any of the files available in this repository, you understand that you are AGREEING TO USE AT YOUR OWN RISK.

# License
[GPL v3][GPL]

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

   [GoCo]: <https://contacts.google.com/>
   [GPL]: <https://www.gnu.org/licenses/gpl-3.0.de.html>

