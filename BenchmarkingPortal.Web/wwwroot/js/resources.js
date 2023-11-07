let uploadProgress;
let cancelUploadButton;
let uploadButton;
let filePath;
let fileName;
let upload;
let fileVersion;

function uploadFile(num) {
    uploadProgress = document.getElementById('uploadProgress' + num);
    cancelUploadButton = document.getElementById('cancelUploadButton' + num);
    uploadButton = document.getElementById('uploadButton' + num);
    filePath = document.getElementById('filePath' + num);
    fileName = document.getElementById('fileName' + num);

    const file = document.getElementById('droppedFile' + num).files[0];
    const rootPath = document.getElementById('rootPath' + num).value;
    fileVersion = document.getElementById('fileVersion' + num).value;

    uploadProgress.value = 0;
    uploadProgress.removeAttribute('data');
    uploadProgress.style.display = 'block';
    disableUpload();

    upload = new tus.Upload(file,
        {
            endpoint: 'files/',
            onError: onTusError,
            onProgress: onTusProgress,
            onSuccess: onTusSuccess,
            metadata: {
                name: fileVersion + '+' + file.name,
                contentType: file.type || 'application/octet-stream',
                emptyMetaKey: ''
            },
            headers: {
                'root': rootPath
            }
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
    upload?.abort();
    setProgressTest('Upload aborted');
    uploadProgress.value = 0;
    enableUpload();
}

function resetLocalCache(e) {
    e.preventDefault();
    localStorage.clear();
    alert('Cache cleared');
}

function onTusError(error) {
    alert(error.message.split('#')[1]);
    enableUpload();
}

function onTusProgress(bytesUploaded, bytesTotal) {
    uploadProgress.value = (bytesUploaded / bytesTotal * 100).toFixed(2);
    setProgressTest(bytesUploaded + '/' + bytesTotal + ' bytes uploaded');
}

function onTusSuccess() {
    filePath.value = upload.url.split('/').pop();
    fileName.value = upload.file.name;
    enableUpload();
}

function setProgressTest(text) {
    uploadProgress.setAttribute('data-label', text);
}

function enableUpload() {
    uploadButton.removeAttribute('disabled');
    cancelUploadButton.setAttribute('disabled', 'disabled');
}

function disableUpload() {
    uploadButton.setAttribute('disabled', 'disabled');
    cancelUploadButton.removeAttribute('disabled');
}