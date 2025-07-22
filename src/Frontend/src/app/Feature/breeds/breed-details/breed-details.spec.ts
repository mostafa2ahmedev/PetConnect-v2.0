import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BreedDetails } from './breed-details';

describe('BreedDetails', () => {
  let component: BreedDetails;
  let fixture: ComponentFixture<BreedDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BreedDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BreedDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
