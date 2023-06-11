<template>
  <v-container>
    <v-card class="orange_background">
      <v-card-title class="pa-7">
        <h2>Create new course</h2>
      </v-card-title>
      <v-card-text>
        <validation-observer v-slot="{ handleSubmit, invalid }">
          <v-form @submit.prevent="handleSubmit(addCourse)">
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
                class="pa-6"
                color="black"
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
              class="pa-6"
              color="black"
              required
            ></v-select>
            <v-row justify="center" class="pa-8">
              <v-btn
                width="50%"
                @click="addCourse"
                :disabled="invalid || selectedSpecialization === 0"
                class="blue-grey lighten-4 pa-3"
                >Submit</v-btn
              ></v-row
            >
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
import { CreateCourseParameters } from "@/modules/commands-parameters";
import { Toastification } from "@/plugins/vue-toastification";
import { SpecializationViewModule } from "@/modules/view-modules";

export default Vue.extend({
  name: "CreateCourseComponent",
  data() {
    return {
      rules,
      name: "",
      selectedSpecialization: 0,
    };
  },
  computed: {
    specializations(): SpecializationViewModule[] {
      return storeHelper.userStore.currentUser.specializations;
    }
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
      } as CreateCourseParameters);

      if (response) {
        Toastification.success("The course was created!");
        this.name='';
        this.selectedSpecialization = 0;
        this.$emit("save");
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
        