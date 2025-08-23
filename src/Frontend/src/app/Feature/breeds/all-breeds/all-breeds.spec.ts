import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllBreeds } from './all-breeds';

describe('AllBreeds', () => {
  let component: AllBreeds;
  let fixture: ComponentFixture<AllBreeds>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllBreeds]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllBreeds);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
