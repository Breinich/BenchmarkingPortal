let uploadProgress;
let cancelUploadButton;
let uploadButton;
let escapeButton;
let okButton;
let cancelButton;
let filePath;
let fileName;
let upload;
let file;

// set source set id for new set file modal
const newSetFileModal = document.getElementById("newSetFile")
if (newSetFileModal) {
    newSetFileModal.addEventListener('show.bs.modal', function (event) {
        const button = event.relatedTarget;
        const sourceSetId = button.getAttribute('data-bs-id');
        const modalSourceSetId = newSetFileModal.querySelector('.modal-body #sourceSetId');
        modalSourceSetId.value = sourceSetId;
    });
}

function uploadFile(num) {
    uploadProgress = document.getElementById('uploadProgress' + num);
    cancelUploadButton = document.getElementById('cancelUploadButton' + num);
    uploadButton = document.getElementById('uploadButton' + num);
    escapeButton = document.getElementById('escapeButton' + num);
    okButton = document.getElementById('okButton' + num);
    cancelButton = document.getElementById('cancelButton' + num);
    filePath = document.getElementById('filePath' + num);
    fileName = document.getElementById('fileName' + num);

    file = document.getElementById('droppedFile' + num).files[0];

    uploadProgress.value = 0;
    uploadProgress.removeAttribute('data');
    uploadProgress.style.display = 'block';
    disableUpload();
    
    let headers = {
        'type': null,
        'sourceSetId': null,
        'version': null
    }

    if (file.name === undefined) {
        return;
    }
    
    let filename = file.name
    
    switch (num) {
        case 1:
            headers.type = 'exe';
            headers.version = document.getElementById('exeVersion').value;
            filename = headers.version + '_' + filename;
            break;
        case 2:
            headers.type = 'sourceSet';
            break;
        case 3:
            headers.type = 'set';
            headers.sourceSetId = document.getElementById('sourceSetId').value;
            filename = headers.sourceSetId + '_' + filename;
            break;
    }

    upload = new tus.Upload(file,
        {
            endpoint: 'files/',
            onError: onTusError,
            onProgress: onTusProgress,
            onSuccess: onTusSuccess,
            metadata: {
                name: filename,
                contentType: file.type || 'application/octet-stream',
                emptyMetaKey: ''
            },
            headers: headers
        });

    setProgressTest('Starting upload...');
    
    upload.findPreviousUploads().then(function (previousUploads) {

        if (previousUploads.length) {
            upload.resumeFromPreviousUpload(previousUploads[0]);
        }
        upload.start();

    }).catch(function () {
        upload.start();
    });
}

function cancelUpload() {
    upload?.abort(true);
    setProgressTest('Upload aborted');
    uploadProgress.value = 0;
    resetLocalCache();
    enableUpload();
    okButton.setAttribute('disabled', 'disabled');
}

function resetLocalCache() {
    localStorage.clear();
    console.log('Cache cleared');
}

function onTusError(error) {
    debugger;
    if ('#' in error.message)
        alert(error.message.split('#')[1]);
    else
        alert(error.message);
    cancelUpload();
}

function onTusProgress(bytesUploaded, bytesTotal) {
    uploadProgress.value = (bytesUploaded / bytesTotal * 100).toFixed(2);
    setProgressTest(bytesUploaded + '/' + bytesTotal + ' bytes uploaded');
}

function onTusSuccess() {
    filePath.value = upload.url.split('/').pop();
    fileName.value = upload.file.name;
    enableUpload();
    okButton.removeAttribute('disabled');
}

function setProgressTest(text) {
    uploadProgress.setAttribute('data-label', text);
}

function enableUpload() {
    uploadButton.removeAttribute('disabled');
    cancelButton.removeAttribute('disabled');
    escapeButton.removeAttribute('disabled');
    cancelUploadButton.setAttribute('disabled', 'disabled');
    okButton.setAttribute('disabled', 'disabled');
}

function disableUpload() {
    uploadButton.setAttribute('disabled', 'disabled');
    cancelButton.setAttribute('disabled', 'disabled');
    escapeButton.setAttribute('disabled', 'disabled');
    cancelUploadButton.removeAttribute('disabled');
}