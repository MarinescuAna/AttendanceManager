/* eslint-disable @typescript-eslint/no-empty-interface */
import Vue from 'vue'
import Vuex from 'vuex'
import document, { namespace as documentNamespace} from "./modules/document/index";
import department, { namespace as departmentNamespace} from "./modules/department";
import user, { namespace as userNamespace} from "./modules/user";
import course, { namespace as courseNamespace} from "./modules/course";
import specialization, {namespace as specializationNamespace} from "./modules/specialization";
import involvement, {namespace as involvementNamespace} from "./modules/involvement/index";
import createPersistedState from "vuex-persistedstate";

//Load Vuex
Vue.use(Vuex)

export interface RootState {}

const modules: { [id: string]: any } = {};

modules[documentNamespace] = document;
modules[departmentNamespace] = department;
modules[userNamespace] = user;
modules[courseNamespace] = course;
modules[specializationNamespace] = specialization;
modules[involvementNamespace] = involvement;

//Create store
export default new Vuex.Store<RootState>({
  state:<RootState>{},
  getters: {
  },
  mutations: {
  },
  actions: {
  },
  modules: modules,
  plugins: [createPersistedState()]
})
