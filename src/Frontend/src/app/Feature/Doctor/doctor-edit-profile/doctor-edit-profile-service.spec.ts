import { TestBed } from '@angular/core/testing';

import { DoctorEditProfileService } from './doctor-edit-profile-service';

describe('DoctorEditProfileService', () => {
  let service: DoctorEditProfileService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DoctorEditProfileService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
