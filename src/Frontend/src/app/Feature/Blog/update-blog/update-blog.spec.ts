import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateBlog } from './update-blog';

describe('UpdateBlog', () => {
  let component: UpdateBlog;
  let fixture: ComponentFixture<UpdateBlog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateBlog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateBlog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
