import { TestBed } from '@angular/core/testing';

import { TimeSlotsCustomerService } from './time-slots-customer-service';

describe('TimeSlotsCustomerService', () => {
  let service: TimeSlotsCustomerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TimeSlotsCustomerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
