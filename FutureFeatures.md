# Features to include later with priorities

### Benchmark handling

Start
- [ ] 2 | Option for rerunning an already existing benchmark
- [ ] 2 | Option for starting a benchmark via uploading the xml configuration file
- [ ] 1 | Fix sv-benchmarks path to relative path based on the xml path and don't copy the xml file
  
Modify
- [ ] 1 | Add vcloud ID to the benchmark table
- [ ] 2 | Option for changing the priority of the benchmark
- [ ] 1 | Option for abort benchmarking tasks
- [ ] 2 | Provide realtime updates about the user's benchmarks in the frontend
  - Realtime messages piped out from the benchmark log
  - Realtime status update (done/remaining, started/finished, etc.)

Durability
- [ ] 2 | Storing the benchmarking tasks persistently in a database
- [ ] 2 | Checking if the benchmarking tasks are already existing in the database and
restarting them, if they are not running >> this is needed for the case that the server is
restarted
- [ ] 2 | Specify Name attributes to the created threads for further supportability

Results
- [X] 1 | Downloading the results properly

UX
- [ ] 1 | Redesign UI, reomve useless components
  - Remove Progress column
  - Remove non-functional buttons
  - Remove the absolut part of the set file's path on Home and Finished pages
  - Create a window width dependant design for the start benchmark modal
  - Move priority button's place to the end of the row
  - Finished header alignment

### Worker handling

Info
- [ ] 2 | Display statistics about the workers
- [ ] 1 | Add vcloud ID to the worker table

Add
- [ ] 1 | Option for adding a new worker
- [ ] 1 | Gather CPU and RAM metadata from vcloud

Modify  
- [ ] 1 | Option for removing a worker
- [ ] 2 | Option for hibernating a worker
- [ ] 2 | Option for waking up a worker

### Resource handling

UX
- [ ] 2 | During uploading the main Add, Cancel and X buttons should be disabled
- [ ] 2 | After cancelling the upload the file should be deleted from the server
- [ ] 2 | Pop-up warning before deleting anything
