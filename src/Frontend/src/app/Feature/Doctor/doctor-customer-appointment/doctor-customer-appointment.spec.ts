import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorCustomerAppointment } from './doctor-customer-appointment';

describe('DoctorCustomerAppointment', () => {
  let component: DoctorCustomerAppointment;
  let fixture: ComponentFixture<DoctorCustomerAppointment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorCustomerAppointment]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorCustomerAppointment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
