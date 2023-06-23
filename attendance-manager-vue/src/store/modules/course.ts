/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { AxiosResponse } from "axios";
import https from "@/plugins/axios";
import { COURSE_CONTROLLER } from "@/shared/constants";
import { CreateCourseParameters, UpdateCourseParameters } from "@/modules/commands-parameters";
import { CourseViewModule } from "@/modules/view-modules";

//state type
export interface CourseState {
    courses: CourseViewModule[]
}

//initialize the state with an empty array
function initialize(): CourseState {
    return {
        courses: []
    };
}

// initial state
const state: CourseState = initialize();

// getters for this store
const getters = {
    /**
     * Gets courses from the store
     */
    courses(state): CourseViewModule[] {
        return state.courses;
    }
};

// mutations for this store
const mutations = {
    /**
     * Update the entire list of courses existed
     */
    _courses(state, courses: CourseViewModule[]): void {
        state.courses = courses;
    },
    /**
     * Add a new course 
     */
    _addCourse(state, course: CourseViewModule): void {
        state.courses.push(course);
    },
    /**
     * Remove course 
     */
    _removeCourse(state, courseId: number): void {
        state.courses = state.courses.filter(cr => cr.courseId != courseId);
    },
    /**
     * Update course name
     */
    _updateCourse(state, payload: CourseViewModule): void {
        state.courses.forEach(cr => {
            if (cr.courseId == payload.courseId) {
                cr.name = payload.name;
                cr.specializationName = payload.specializationName;
                cr.specializationId = payload.specializationId;
            }
        });
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void {
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions = {
    /**
     * Reset the state with the initial values
     */
    resetStore({ commit }): void {
        commit('_resetStore');
    },
    /**
     * Load all the courses defined by the current user, not all courses
     */
    async loadCoursesAsync({ commit, state }, reload: boolean): Promise<void> {
        if (state.courses.length != 0 && !reload) {
            return state.courses;
        }

        const courses: CourseViewModule[] = (await https.get(`${COURSE_CONTROLLER}/courses`)).data;

        //sort courses by name
        const sortedCourses = courses.sort((course1, course2) => {
            if (course1.name < course2.name) {
                return -1;
            }
            if (course1.name > course2.name) {
                return 1;
            }
            return 0;
        });

        commit("_courses", sortedCourses);
    },
    /**
     * Add a new course
     */
    async addCourseAsync({ commit }, payload: CreateCourseParameters): Promise<boolean> {
        let isSuccess = true;

        const result = await https.post(`${COURSE_CONTROLLER}/create_course`, payload)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_addCourse", {
                courseId: (result as AxiosResponse).data,
                name: payload.name,
                specializationId: payload.specializationId,
                specializationName: payload.specializationName
            } as CourseViewModule);
        }

        return isSuccess;
    },
    /**
     * Remove a course
     */
    async removeCourseAsync({ commit }, payload: number): Promise<boolean> {
        let isSuccess = true;

        await https.delete(`${COURSE_CONTROLLER}/delete_course/${payload}`)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_removeCourse", payload);
        }
        return isSuccess;
    },
    /**
     * Update course
     */
    async updateCourseAsync({ commit }, payload: { specializationName: string, parameter: UpdateCourseParameters }): Promise<boolean> {
        let isSuccess = true;

        await https.patch(`${COURSE_CONTROLLER}/update_Course`, payload.parameter)
            .catch(error => {
                isSuccess = ResponseHandler.errorResponseHandler(error);
            });

        if (isSuccess) {
            commit("_updateCourse", {
                courseId: payload.parameter.courseId,
                name: payload.parameter.name,
                specializationId:
                    payload.parameter.specializationId,
                specializationName: payload.specializationName
            } as CourseViewModule);
        }
        return isSuccess;
    }
};

// export the namespace
export const namespace = 'course';

// export the store 'module'
export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
};
