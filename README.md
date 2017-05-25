# bmconv

[![AppVeyor](https://img.shields.io/appveyor/ci/hozuki/bmconv.svg)](https://ci.appveyor.com/project/hozuki/bmconv)
[![GitHub contributors](https://img.shields.io/github/contributors/OpenCGSS/bmconv.svg)](https://github.com/OpenCGSS/bmconv/graphs/contributors)
[![Libraries.io for GitHub](https://img.shields.io/librariesio/github/OpenCGSS/bmconv.svg)](https://github.com/OpenCGSS/bmconv)
[![Github All Releases](https://img.shields.io/github/downloads/OpenCGSS/bmconv/total.svg)](https://github.com/OpenCGSS/bmconv/releases)

A utility program that converts CGSS beatmap to other formats.

Currently it supports:

- Deleste (TXT, format 2)

## Usage

```plain
bmconv <input file> [options]

Options:

  -t, --to        (Default: txt) Conversion type. Available: txt.

  --difficulty    (Default: 0) The specified difficulty when opening a beatmap
                  bundle (BDB).

  -o, --out       Output file location.
```

## License

MIT License
