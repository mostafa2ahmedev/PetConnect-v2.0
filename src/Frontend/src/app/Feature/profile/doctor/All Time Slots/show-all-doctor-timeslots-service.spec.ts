import { TestBed } from '@angular/core/testing';

import { ShowAllDoctorTimeslotsService } from './show-all-doctor-timeslots-service';

describe('ShowAllDoctorTimeslotsService', () => {
  let service: ShowAllDoctorTimeslotsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShowAllDoctorTimeslotsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
