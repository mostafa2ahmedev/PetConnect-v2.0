import { TestBed } from '@angular/core/testing';

import { FaceComparisonService } from './face-comparison.service';

describe('FaceComparisonService', () => {
  let service: FaceComparisonService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FaceComparisonService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
