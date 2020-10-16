# BigFileSorter

The project is implement some kind of [external sort](https://en.wikipedia.org/wiki/External_sorting#:~:text=External%20sorting%20is%20a%20class,usually%20a%20hard%20disk%20drive.) for huge files (100 Gb e.g.)
The solunion has two projects: `Generator` and 'Sorter'
Original task is:
  1. Generate a txt file with a custom size and with a format like: '`<NUMBER>`. `<STRING>`'. Number and string can duplicate in result file.
  2. Sort the generated file. Where first sort by the string and then by the number.

## `Generator` desciption

  1. Generator is use `Parallel` class for generate parts of the file and store them in the result. A part size is 1 Gb.
  2. You need more than CPU count in GB of free space RAM. This meen that  free space of RAM need to be more than CPU count in Gb. (For 8 core process need >8 free size of RAM).
  3. And you also need enough free space on you hard disk.
  4. Parameters that you can use:
      * `-s` or `--size`: size of result file in Gigabytes (1..255). By defult - 4 Gb.
      * `o` or `--output`: output file name or path. By default - `<current_folder>`\result_`<current_date_time>`.txt
      * `-?`: help info.

## `Sorter` description

  1. Sorter works in 2 steps:
      * By the first - sorter is separete a input file for sorted chunks, and store them into `Temp` folder.
      * Then `Sorter` merge the chunks.
  2. You need enough free space on the hard disk (at least three times more)
  3. Parameters that you can use:
      * `-i` or `--input`: input file name or path. Requered parameter.
      * `-o` or `--output`: output file name or path. By default - `<current_folder>\Result_`<current_date_time>`.txt
      * `-t` or `--temp`: temp folder path. By default - `<current_folder>\Temp`
      * `-b` or `--buffer`: buffer size in lines count. By default: 12000000
      * `?`: help info
   
## Future plans
  
    1. Need to add a some kind of analyze the current free space of RAM and adapt in memory buffers for both `Generator` and `Sorter`.
    2. Investigate possibility to change or exclude serialization or deserialization of lines and improve  compare algorithm for `Record` objects.
    3. To use the `bytes` as a value for `Sorter` buffers.
