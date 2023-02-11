/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { CourseViewModule, CreateCourseModule, UpdateCourseModule } from "@/modules/course";
import { ResponseModule } from "@/shared/modules";
import { AxiosResponse } from "axios";
import {Logger} from "@/plugins/custom-plugins/logging";
import https from "@/plugins/axios";
import { COURSE_CONTROLLER } from "@/shared/constants";

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
    _removeCourse(state, courseId: string): void {
        state.courses = state.courses.filter(cr => cr.courseId != courseId);
    },
    /**
     * Update course name
     */
    _updateCourse(state, id: string, name: string): void {
        state.courses.foreach(cr =>{
            if(cr.courseId == id){
                cr.name = name;
            }
        });
    },
    /**
     * Reset the state with the initial values
     */
    _resetStore(state): void{
        Object.assign(state, initialize());
    }
};

// actions for this store
const actions = {
    /**
     * Reset the state with the initial values
     */
    resetStore({commit}):void{
        commit('_resetStore');
    },
    /**
     * Load all the courses defined by the current user, not all courses
     */
    async loadCourses({ commit, state }): Promise<void> {
        if(state.courses.length !=0){
            return state.courses;
        }
        
        const courses: CourseViewModule[] = (await https.get(`${COURSE_CONTROLLER}/courses`)).data;
        commit("_courses", courses);
    },
    /**
     * Add a new course
     */
    async addCourse({ commit }, payload: CreateCourseModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };
        const result = await https.post(`${COURSE_CONTROLLER}/create_course`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_addCourse", {
                id: (result as AxiosResponse).data,
                name: payload.name,
                specializationId: payload.specializationId,
                specializationName: payload.specializationName
            } as CourseViewModule);
        }
        return response;
    },
    /**
     * Remove a course
     */
    async removeCourse({ commit }, payload: number): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        await https.patch(`${COURSE_CONTROLLER}/delete_course?id=${payload}`)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_removeCourse", payload);
        }
        return response;
    },
    /**
     * Update course
     */
     async updateCourse({ commit }, payload: UpdateCourseModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };

        await https.patch(`${COURSE_CONTROLLER}/update_Course`, payload)
            .catch(error => {
                response = ResponseHandler.errorResponseHandler(error);
            });

        if (response.isSuccess) {
            commit("_updateCourse", payload.courseId, payload.name);
        }
        return response;
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
