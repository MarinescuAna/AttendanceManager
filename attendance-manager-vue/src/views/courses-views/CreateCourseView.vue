<template>
  <v-container>
    <v-card class="orange lighten-3">
      <v-card-title class="pa-3">
        <h2>Create new course</h2>
        <v-spacer></v-spacer>
        <router-link to="/courses" tag="button"
          ><v-icon>mdi-close</v-icon></router-link
        >
      </v-card-title>
      <v-card-text>
        <validation-observer v-slot="{ handleSubmit, invalid }">
          <v-form @submit.prevent="handleSubmit(addCourse)">
            <v-layout column>
              <validation-provider
                name="name"
                v-slot="{ errors }"
                :rules="rules.required"
              >
                <v-text-field
                  v-model="name"
                  type="text"
                  label="Course name"
                  maxlength="128"
                  prepend-icon="mdi-pencil"
                  :error-messages="errors"
                  required
                  counter
                />
              </validation-provider>
              <v-select
                :items="specializations"
                label="Specializations"
                v-model="selectedSpecialization"
                prepend-icon="mdi-file"
                item-text="name"
                item-value="id"
                :disabled="specializations.length == 0"
                required
              ></v-select>
              <v-btn
                width="40%"
                @click="addCourse"
                :disabled="invalid || selectedSpecialization === 0"
                class="blue-grey lighten-4 pa-3"
                >Submit</v-btn
              >
            </v-layout>
          </v-form>
        </validation-observer>
      </v-card-text>
    </v-card>
  </v-container>
</template>
    
    
    <script lang="ts">
import Vue from "vue";
import { rules } from "@/plugins/vee-validate";
import storeHelper from "@/store/store-helper";
import { CreateCourseModule } from "@/modules/course";
import { SpecializationModule } from "@/modules/specialization";

export default Vue.extend({
  name: "CreateCourseView",
  data() {
    return {
      rules,
      // Course name
      name: "",
      // Selected specializations
      selectedSpecialization: 0,
    };
  },
  computed: {
    // All the specializations
    specializations(): SpecializationModule[] {
      return storeHelper.userStore.currentUser.specializations;
    },
  },
  methods: {
    /**
     * Use this method for adding a new specialization
     * Success: reset the form and reload the treeview
     */
    async addCourse() {
      const response = await storeHelper.courseStore.addCourse({
        name: this.name,
        specializationId: this.selectedSpecialization,
        specializationName: this.specializations.find(
          (x) => x.id == this.selectedSpecialization
        )!.name,
      } as CreateCourseModule);

      if (response) {
        this.$router.currentRoute.meta?.onBack();
      }
    },
    /**
     * Call this function whenever you reset the department selector
     */
    onResetSpecializations(): void {
      this.specializations = [];
      this.selectedSpecialization = 0;
    },
  },
});
</script>
      