<template>
  <v-card class="pa-4">
    <validation-observer v-slot="{ handleSubmit, invalid }">
      <v-form @submit.prevent="handleSubmit(onSubmitAsync)">
        <v-layout column class="pa-2">
          <v-card-title>
            <p>Use involvement code</p>
            <v-spacer></v-spacer>
            <v-btn icon @click="onClose">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-text>
            <validation-provider
              name="code"
              v-slot="{ errors }"
              :rules="rules.required"
            >
              <v-text-field
                v-model="code"
                label="Enter the code"
                :error-messages="errors"
                maxlength="8"
                color="black"
                minlength="6"
                prepend-icon="mdi-lock"
              ></v-text-field>
            </validation-provider>
          </v-card-text>
          <v-card-actions>
            <v-layout class="pa-2" justify-center row>
              <v-btn
                class="mr-1 dark_button white--text"
                :disabled="invalid"
                @click="onSubmitAsync"
              >
                Submit
              </v-btn>
            </v-layout>
          </v-card-actions>
        </v-layout>
      </v-form>
    </validation-observer>
  </v-card>
</template>
  
  <script lang="ts">
import { rules } from "@/plugins/vee-validate";
import Vue from "vue";
import InvolvementService from "@/services/involvement.service";
import { Toastification } from "@/plugins/vue-toastification";

export default Vue.extend({
  name: "UseInvolvementCodeDialog",
  props: {
    attendanceId: Number,
    collectionId: Number
  },
  data() {
    return {
      rules,
      code: "",
    };
  },
  methods: {
    onClose: function (): void {
      this.$emit("close");
    },
    onSubmitAsync: async function (): Promise<void> {
      const result =
        await InvolvementService.updateAttendanceByCodeAsync({
            code: this.code,
            attendanceId: this.attendanceId,
            collectionId: this.collectionId
        });

      if (result) {
        Toastification.success("The attendance was successfully updated!");
        this.$emit("save");
      }
    },
  },
});
</script>