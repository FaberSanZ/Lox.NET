# 


<h1 align="center">
   Lox.NET
  <br>
  
 ## A cross-platform compiler/interpreter .NET Standard implementation of the [Lox](https://github.com/munificent/craftinginterpreters) language Roslyn inspired.  
</h1>


<br>

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Zeckoxe/Z-Sharp/blob/master/LICENSE)

The code is licensed under MIT. Feel free to use it for whatever purpose.

<hr>
<br>




## Examples

```csharp

class Console 
{
    Console(str) 
    {
	    this.name = str;
    }

    WriteDebug() 
    {
        print this.name;
    }

    Write(str) 
    {
        print str;
    }
}

class Program
{
    Program()
    {

    }

    Main()
    {
        let cw = Console("Debug Init");

        cw.Write("Hello Word 0");
        cw.Write("Hello Word 1");
        cw.Write("Hello Word 2");
        cw.Write("Hello Word 3");
        cw.WriteDebug();
    }
}
```

