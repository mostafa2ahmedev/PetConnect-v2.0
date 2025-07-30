import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowAllDoctorTimeslots } from './show-all-doctor-timeslots';

describe('ShowAllDoctorTimeslots', () => {
  let component: ShowAllDoctorTimeslots;
  let fixture: ComponentFixture<ShowAllDoctorTimeslots>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShowAllDoctorTimeslots]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowAllDoctorTimeslots);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
