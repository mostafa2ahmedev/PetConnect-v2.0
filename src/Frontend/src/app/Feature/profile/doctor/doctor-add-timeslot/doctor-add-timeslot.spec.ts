import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorAddTimeslot } from './doctor-add-timeslot';

describe('DoctorAddTimeslot', () => {
  let component: DoctorAddTimeslot;
  let fixture: ComponentFixture<DoctorAddTimeslot>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorAddTimeslot]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorAddTimeslot);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
