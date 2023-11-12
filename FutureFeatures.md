# Features to include later

### Benchmark handling

- [ ] Option for rerunning an already existing benchmark
- [ ] Option for starting a benchmark via uploading the xml configuration file
- [ ] Option for abort benchmarking tasks
- [ ] Storing the benchmarking tasks persistently in a database
- [ ] Checking if the benchmarking tasks are already existing in the database and restarting them,
  if they are not running >> this is needed for the case that the server is restarted
- [ ] Provide realtime updates about the user's benchmarks in the frontend
  - Realtime messages piped out from the benchmark log
  - Realtime status update (done/remaining, started/finished, etc.)
- [ ] Redesign UI, reomve useless components
  - Remove Progress column
  - Remove non-functional buttons
  - Remove the absolut part of the set file's path
  - Create window width dependant design for the start benchmark modal
  - Move priority button's place to the end of the row
  - Finished header alignment

### Worker handling

- [ ] Display statistics about the workers

### Resource handling

- [ ] During uploading the main Add, Cancel and X buttons should be disabled