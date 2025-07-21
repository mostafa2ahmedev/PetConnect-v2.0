import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBreed } from './add-breed';

describe('AddBreed', () => {
  let component: AddBreed;
  let fixture: ComponentFixture<AddBreed>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBreed]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddBreed);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
