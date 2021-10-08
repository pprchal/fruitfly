# FruitFly - really tiny static blog system
If you're **bored** with gigantic blogging systems.
~o~
Basics: 
* Fruitfly take `.md` file and converts it into `.html`
* Templates can be recurred, look into `{variables}`

## Quick steps
1) put your data into your `blog_input` folder i.e: `C:\myblog\blog_input`
2) modify `config.yml` - is well commented and simple
3) run `dotnet run` in directory which contains `config.yml` folder and/or `dotnet run config.yml`

```
PS C:\FruitFly> dotnet run
~o~ FRUITFLY 1.0 Blog generator
        ~o~ c:\myblog\blog_input\y2019\m11\d15_post1
        ~o~ c:\myblog\blog_input\y2019\m11\d30_post1
2 ~o~ generated at: $0,7855702 second(s)
```

pretty simple, huh?

## Required structure
`y{year}/m{month}/d{day}_post{n}`

Example:
```
C:.
├───blog_input
│   └───y2019
│       └───m11
│           ├───d15_post1
│           └───d30_post1
└───y2019                    <---- output directory
    └───m11
        ├───d15_post1
        └───d30_post1
```


## Dev notes
Update package `dotnet add package YamlDotNet`


## Future
Is very clear. I'm too lazy.

* better documentation
* better distribution
* better....
* high-async support - as test project