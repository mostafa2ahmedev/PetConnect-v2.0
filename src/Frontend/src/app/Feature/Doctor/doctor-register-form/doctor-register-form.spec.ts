import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorRegisterForm } from './doctor-register-form';

describe('DoctorRegisterForm', () => {
  let component: DoctorRegisterForm;
  let fixture: ComponentFixture<DoctorRegisterForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorRegisterForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorRegisterForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
