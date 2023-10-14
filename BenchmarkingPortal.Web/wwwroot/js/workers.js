const editComputerGroupModal = document.getElementById('editCG');
if (editComputerGroupModal) {
    editComputerGroupModal.addEventListener('show.bs.modal', function (event) {
        const button = event.relatedTarget;
        const computerGroupId = button.getAttribute('data-bs-id');
        const computerGroupDesc = button.getAttribute('data-bs-desc');
        const modalTitle = editComputerGroupModal.querySelector('.modal-title');
        const modalBodyInput = editComputerGroupModal.querySelector('.modal-body #editDesc');
        const modalEditId = editComputerGroupModal.querySelector('.modal-body #editId');

        modalTitle.textContent = 'Edit Computer Group: ' + computerGroupId;
        modalBodyInput.value = computerGroupDesc;
        modalEditId.value = computerGroupId;
    });
}