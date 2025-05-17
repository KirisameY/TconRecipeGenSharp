# TconRecipeGenSharp

\[[简体中文](ReadMe.md)\] | \[English\]

# How to use

Drag and drop one or more input files to the program, the program will automatically parse the file, and generate corresponding data files in the working directory.<br/>
Example of input file:

```toml
[foo]
# Following three entries are required
item = "ns:foo_ingot"
molten = "ns:molten_foo"
material = "ns:foo"
# Following is optional
temperature = 1000
time = 100

[fooo]
# ...
```

---