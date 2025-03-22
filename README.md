How to install:
1. Add openupm scope to manifest.json
```
"scopedRegistries": [ 
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.cysharp.unitask"
      ]
    }
  ]
```

2. Add beton to UPM with link
  https://github.com/vovaBuddy/beton.git?path=Beton#develop


3. For correct work of generators add reference to beton in your project assembly definition file
