$(document).ready(function () {
    loadEmployees();
});


function loadEmployees() {

    $.ajax({
        url: '/Employee/GetAll',
        type: 'GET',

        success: function (employees) {

            let html = '';

            if (!employees || employees.length === 0) {
                html = `
                    <tr>
                        <td colspan="8"
                            class="text-center py-4 text-muted">
                            No employees found.
                        </td>
                    </tr>
                `;

                $('#employeeTableBody').html(html);
                return;
            }

            employees.forEach(function (employee) {

                const statusHtml = employee.isActive
                    ? `
                        <span class="status-badge status-active">
                            <span class="status-dot"></span>
                            Active
                        </span>
                      `
                    : `
                        <span class="status-badge status-inactive">
                            <span class="status-dot"></span>
                            Inactive
                        </span>
                      `;

                html += `
                    <tr>

                        <td>
                            <span class="employee-name">
                                ${employee.name ?? '-'}
                            </span>
                        </td>

                        <td>
                            ${employee.email ?? '-'}
                        </td>

                        <td>
                            ${employee.phone ?? '-'}
                        </td>

                        <td>
                            ${employee.department ?? '-'}
                        </td>

                        <td>
                            ${employee.designation ?? '-'}
                        </td>

                        <td>
                            ${employee.salary ?? '-'}
                        </td>

                        <td>
                            ${statusHtml}
                        </td>

                        <td>

                            <button class="btn btn-sm btn-outline-primary"
                                    onclick="editEmployee(${employee.id})">
                                <i class="bi bi-pencil"></i>
                                Edit
                            </button>

                            <button class="btn btn-sm btn-outline-danger"
                                    onclick="deleteEmployee(${employee.id})">
                                <i class="bi bi-trash"></i>
                                Delete
                            </button>

                        </td>

                    </tr>
                `;
            });

            $('#employeeTableBody').html(html);
        },

        error: function (xhr) {

            console.error('Unable to load employees:', xhr);

            $('#employeeTableBody').html(`
                <tr>
                    <td colspan="8"
                        class="text-center py-4 text-danger">
                        Unable to load employees.
                    </td>
                </tr>
            `);
        }
    });
}


/* =========================
   CREATE
========================= */

function openCreateModal() {

    $('#modalTitle').text('Add Employee');

    $('#employeeId').val('');

    $('#name').val('');
    $('#email').val('');
    $('#phone').val('');
    $('#department').val('');
    $('#designation').val('');
    $('#joiningDate').val('');
    $('#salary').val('');
    $('#isActive').val('true');

    $('#employeeModal').modal('show');
}


/* =========================
   GET BY ID / EDIT
========================= */

function editEmployee(id) {

    $.ajax({
        url: `/Employee/GetById?id=${id}`,
        type: 'GET',

        success: function (employee) {

            $('#modalTitle').text('Edit Employee');

            $('#employeeId').val(employee.id);

            $('#name').val(employee.name);
            $('#email').val(employee.email);
            $('#phone').val(employee.phone);
            $('#department').val(employee.department);
            $('#designation').val(employee.designation);

            if (employee.joiningDate) {

                $('#joiningDate').val(
                    employee.joiningDate.substring(0, 10)
                );
            }
            else {
                $('#joiningDate').val('');
            }

            $('#salary').val(employee.salary);

            $('#isActive').val(
                employee.isActive.toString()
            );

            $('#employeeModal').modal('show');
        },

        error: function (xhr) {

            console.error('Unable to load employee:', xhr);

            alert('Unable to load employee.');
        }
    });
}


/* =========================
   CREATE / UPDATE
========================= */

function saveEmployee() {

    const id = $('#employeeId').val();

    const employee = {

        id: id ? parseInt(id) : 0,

        name: $('#name').val(),

        email: $('#email').val(),

        phone: $('#phone').val(),

        department: $('#department').val(),

        designation: $('#designation').val(),

        joiningDate: $('#joiningDate').val() || null,

        salary: $('#salary').val()
            ? parseFloat($('#salary').val())
            : null,

        isActive: $('#isActive').val() === 'true'
    };


    /* UPDATE */

    if (id) {

        $.ajax({

            url: `/Employee/Update?id=${id}`,

            type: 'PUT',

            contentType: 'application/json',

            data: JSON.stringify(employee),

            success: function () {

                $('#employeeModal').modal('hide');

                loadEmployees();

                alert('Employee updated successfully.');
            },

            error: function (xhr) {

                console.error('Update error:', xhr);

                alert('Unable to update employee.');
            }
        });

    }

    /* CREATE */

    else {

        $.ajax({

            url: '/Employee/Create',

            type: 'POST',

            contentType: 'application/json',

            data: JSON.stringify(employee),

            success: function () {

                $('#employeeModal').modal('hide');

                loadEmployees();

                alert('Employee created successfully.');
            },

            error: function (xhr) {

                console.error('Create error:', xhr);

                alert('Unable to create employee.');
            }
        });
    }
}


/* =========================
   DELETE
========================= */

function deleteEmployee(id) {

    if (!confirm('Are you sure you want to delete this employee?')) {
        return;
    }


    $.ajax({

        url: `/Employee/Delete?id=${id}`,

        type: 'DELETE',

        success: function () {

            loadEmployees();

            alert('Employee deleted successfully.');
        },

        error: function (xhr) {

            console.error('Delete error:', xhr);

            alert('Unable to delete employee.');
        }
    });
}