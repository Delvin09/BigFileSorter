# BigFileSorter

The solution has two projects: `Generator` and `Sorter`.
The `Sorter` project implements some kind of [external sorting algorithm](https://en.wikipedia.org/wiki/External_sorting#:~:text=External%20sorting%20is%20a%20class,usually%20a%20hard%20disk%20drive.) for huge files (e.g. 100GB).
Its original task is:
  1. To generate a txt file of custom size and format like: '`<NUMBER>`. `<STRING>`'. Number and string can be duplicated in the result file.
  2. To sort the generated file, first sorting by string and then by number.

## `Generator` description

  1. Generator uses `Parallel` class to generate parts of a file and store them in the result. Part size is 1GB.
  2. You need more RAM free space than the number of CPUs. (An 8-core processor needs >8 free GBs of RAM).
  3. You also need enough free space on your hard drive.
  4. Parameters you can use:
      * `-s` or `--size`: size of the result file in Gigabytes (1..255). By default - 4 Gb.
      * `o` or `--output`: output file name or path. By default - `<current_folder>`\result_`<current_date_time>`.txt
      * `-?`: help info.

## `Sorter` description

  1. Sorter works in 2 steps:
      * First, sorter breaks down an input file into sorted chunks, and stores them into `Temp` folder.
      * Then `Sorter` merges the chunks.
  2. You need enough free space on the hard drive (at least three-fold).
  3. Parameters you can use:
      * `-i` or `--input`: input file name or path. Required parameter.
      * `-o` or `--output`: output file name or path. By default - `<current_folder>\Result_`<current_date_time>`.txt
      * `-t` or `--temp`: temp folder path. By default - `<current_folder>\Temp`
      * `-b` or `--buffer`: buffer size in line count. By default: 12,000,000
      * `?`: help info
   
## Future plans
  
  1. To add some kind of a feature to analyze the current free space of RAM and adapt in-memory buffers for both `Generator` and `Sorter`.
  2. To investigate the possibility of changing or excluding line serialization or deserialization and improving comparison algorithm for `Record` objects.
  3. To use the `bytes` as a value for `Sorter` buffers.
