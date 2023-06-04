import {CourseType } from "@/shared/enums";

export interface CreateBadgeParameters{
    maxNumber: number;
    title: string;
    badgeType: string;
    type: CourseType;
}