/* eslint-disable @typescript-eslint/no-explicit-any */
import ResponseHandler from "@/error-handler/error-handler";
import { CourseViewModule, CreateCourseModule, UpdateCourseModule, CreateCourseDto } from "@/modules/course";
import { ResponseModule } from "@/shared/modules";
import axios, { AxiosResponse } from "axios";

//state type
export interface CourseState {
    courses: CourseViewModule[]
}

//initialize the state with an empty array
export function initialize(): CourseState {
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

        if (state.courses.length == 0) {
            actions.loadCourses;
        }

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
    }

};

// actions for this store
const actions = {
    /**
     * Load all the courses
     */
    async loadCourses({ commit }): Promise<CourseViewModule[]> {
        const courses: CourseViewModule[] = (await axios.get('course/courses')).data;
        commit("_courses", courses);
        return courses;
    },
    /**
     * Add a new course
     */
    async addCourse({ commit }, payload: CreateCourseModule): Promise<ResponseModule> {
        let response: ResponseModule = {
            error: "",
            isSuccess: true
        };
        const result = await axios.post(`course/create_course`, {
            name: payload.name,
            specializationId: payload.specializationId
        } as CreateCourseDto)
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

        await axios.patch(`course/delete_course?id=${payload}`)
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

        await axios.patch(`course/update_Course`, payload)
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
