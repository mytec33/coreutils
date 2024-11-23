package main

import (
	"flag"
	"fmt"
	"os"
	"time"
)

const (
	EXIT_SUCCESS = 0
	EXIT_FAILURE = 1
)

func main() {
	var dashA bool
	var dashC bool
	var dashM bool
	var helpFlag bool
	var versionFlag bool

	flag.BoolVar(&dashA, "a", false, "change only the access time")
	flag.BoolVar(&dashC, "c", false, "do not create any files")
	flag.BoolVar(&dashM, "m", false, "change only the modification time")
	flag.BoolVar(&helpFlag, "help", false, "display this help and exit")
	flag.BoolVar(&versionFlag, "version", false, "output version information and exit")
	flag.Parse()

	if helpFlag {
		help()
		os.Exit(EXIT_SUCCESS)
	}

	if versionFlag {
		version()
		os.Exit(EXIT_SUCCESS)
	}

	files := flag.Args()
	if len(files) == 0 {
		os.Exit(EXIT_FAILURE)
	}

	// The same time is used for all files in this invocation
	currentTime := time.Now()
	var accessTime time.Time
	var modificationTime time.Time

	if !dashA && !dashM {
		accessTime = currentTime
		modificationTime = currentTime
	} else {
		if dashA {
			accessTime = currentTime
		}

		if dashM {
			modificationTime = currentTime
		}
	}

	updateFileTimes(files, accessTime, modificationTime, dashC)
}

func updateFileTimes(files []string, accessTime time.Time, modificationTime time.Time, noCreate bool) {
	// Note: viewing `stat` output, change time will change as well as the access time

	flags := os.O_RDONLY
	if !noCreate {
		flags |= os.O_CREATE
	}
	for _, f := range files {
		// This is as close as I can get to having ctime sync with access and/or modification times
		fd, err := os.OpenFile(f, flags, 0644)
		if err != nil {
			fmt.Printf("%v", err.Error())
		}

		err = os.Chtimes(f, accessTime, modificationTime)
		if err != nil {
			fmt.Printf("%v", err.Error())
		}

		// Saving closing to the end to help keep Chtimes closer to ctime by OS
		fd.Close()
	}
}

func help() {
	fmt.Println("Usage: touch [OPTION]... FILE...")
	fmt.Println("Update the access and modification times of each FILE to the current time.")
	fmt.Println()
	fmt.Println("A FILE argument that does not exist is created empty, unless -c or -h is supplied.")
}

func version() {
	fmt.Println("golang_touch 1.0")
}
