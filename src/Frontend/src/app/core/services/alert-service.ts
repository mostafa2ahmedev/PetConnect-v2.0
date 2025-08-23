import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  success(message: string, title: string = 'Success') {
    Swal.fire({
      icon: 'success',
      title,
      text: message,
      timer: 2000,
      showConfirmButton: false,
    });
  }

  error(message: string, title: string = 'Error') {
    Swal.fire({
      icon: 'error',
      title,
      text: message,
    });
  }

  confirm(
    text: string = 'Are you sure?',
    title: string = 'Confirm',
    confirmText: string = 'Yes',
    cancelText: string = 'Cancel'
  ): Promise<boolean> {
    return Swal.fire({
      title,
      text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: confirmText,
      cancelButtonText: cancelText,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
    }).then((result) => result.isConfirmed);
  }
}
