import { TestBed } from '@angular/core/testing';

import { DoctorCustomerAppointmentService } from './doctor-customer-appointment-service';

describe('DoctorCustomerAppointmentService', () => {
  let service: DoctorCustomerAppointmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DoctorCustomerAppointmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
