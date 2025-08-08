import { TestBed } from '@angular/core/testing';

import { DoctorAddTimeslotService } from './doctor-add-timeslot-service';

describe('DoctorAddTimeslotService', () => {
  let service: DoctorAddTimeslotService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DoctorAddTimeslotService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
