
import { AttendanceCollectionViewModule } from "@/modules/document/attendance-collection";
import { BadgeType } from "@/shared/enums";

/** Use this view only for sending the collected data from the form used for creating a new document, to the API */
export interface DocumentInsertModule{
    title: string;
    courseId: number;
    specializationId: number;
    enrollmentYear: number;
    maxNoLessons: number;
    maxNoLaboratories: number;
    maxNoSeminaries: number;
    studentIds: string[];
    attendanceImportance: number;
    bonusPointsImportance: number;
}

/** Use this view only for sending the collected data from the form used for updating a document, to the API */
export interface DocumentUpdateModule{
    title: string;
    courseId: number;
    noLessons: number;
    noLaboratories: number;
    noSeminaries: number;
    attendanceImportance: number;
    bonusPointsImportance: number;
    documentId: number;
}


/** Use this view in order to get the documents for the cards */
export interface DocumentViewModule{
    title: string;
    courseName: string;
    specializationName: string;
    enrollmentYear: number;
    documentId: number;
    isCreator: boolean;
    updatedOn: string;
}

export interface DocumentMembersViewModule{
    email: string;
    name: string;
}

export interface BadgeViewModule{
    imagePath: string;
    type: BadgeType;
    title: string;
    rewardId: number;
    badgeId: number;
    isActive: boolean;
}

/** This module is used in the document store to keep all the informations related to a document */
export interface DocumentFullViewModule{
    documentId:number;
    title: string;
    courseId: number;
    courseName: string;
    specializationId: number;
    specializationName: string;
    enrollmentYear: number;
    maxNoLessons: number;
    maxNoLaboratories: number;
    maxNoSeminaries: number;
    noLessons: number;
    noLaboratories: number;
    noSeminaries: number;
    creationDate: string;
    updateOn: string;
    createdBy: string;
    attendanceCollections: AttendanceCollectionViewModule[];
    documentMembers: DocumentMembersViewModule[];
    attendanceImportance: number;
    bonusPointsImportance: number;
    numberOfStudents: number;
}