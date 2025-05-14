import sys
from   langchain.llms import LlamaCpp

# enable verbose to debug the LLM's operation
verbose = False

class MyLLM:

    def __init__(self):
        self.model = LlamaCpp(
            model_path = "/Users/fabian-mac/Desktop/Job_ETH/Korsika_prototype_resources/Resources/llama-3.2-1b-instruct-q4_k_m.gguf",

            # max tokens the model can account for when processing a response
            max_tokens = 4096,

            # number of layers to offload to the GPU 
            n_gpu_layers = 32,

            # number of tokens in the prompt that are fed into the model at a time
            n_batch = 1024,

            # use half precision for key/value cache; set to True per langchain doc
            f16_kv = True,
            verbose = verbose,
        )
        
    def performInference(self, prompt):
        output = self.model(
            prompt,
            max_tokens = 4096,
            temperature = 0.2,
            # nucleus sampling (mass probability index)
            # controls the cumulative probability of the generated tokens
            # the higher top_p the more diversity in the output
            top_p = 0.1
        )
        return output

print("START")
llm = MyLLM()
print(llm.performInference("Tell me about your day"))

