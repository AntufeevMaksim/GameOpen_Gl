#version 440

out vec4 outputColor;

in vec2 texCoord;
in vec3 Normal;
in vec3 FragPos;


uniform vec3 lightPos;
uniform vec3 viewPos;


struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;
    float     shininess;
}; 

struct PointLight {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

struct DirectionalLight{
    vec3 direction;

    vec3 diffuse;
    vec3 specular;    
};

struct Spotlight{
    vec3 position;
    vec3 direction;
    float phi;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;

uniform PointLight pLight;
uniform DirectionalLight dirLight;
uniform Spotlight spotlight;

void main()
{
    vec3 ambient = vec3(0.0f);
    vec3 diffuse = vec3(0.0f);
    vec3 specular = vec3(0.0f);

    float distance = length(FragPos - pLight.position);
    float attenuation = 1.0f / (pLight.constant + pLight.linear * distance + pLight.quadratic * (distance * distance));

    vec3 norm = normalize(Normal);
    // ambient
    ambient += pLight.ambient * vec3(texture(material.diffuse, texCoord)) * attenuation;

    // directional light
    //diffuse
    vec3 lightDir = normalize(-dirLight.direction);
    float diff = max(dot(norm, lightDir), 0.0);
    diffuse += dirLight.diffuse * diff * vec3(texture(material.diffuse, texCoord));


    // point light
    // diffuse
    lightDir = normalize(pLight.position - FragPos);
    diff = max(dot(norm, lightDir), 0.0);
    diffuse += pLight.diffuse * diff * vec3(texture(material.diffuse, texCoord)) * attenuation;

    // specular
    float specularStrength = 0.5f;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    specular += vec3(texture(material.specular, texCoord)) * spec * pLight.specular  * attenuation;


    //spotlight
    //diffuse
    lightDir = normalize(spotlight.position - FragPos);
    float theta = dot(lightDir, normalize(-spotlight.direction));
    if (theta >= spotlight.phi)
    {
        diff = max(dot(norm, lightDir), 0.0);
//        diffuse += spotlight.diffuse * diff * vec3(texture(material.diffuse, texCoord)) * theta;        
    }

    vec3 emission = vec3(texture(material.emission, texCoord)).rgb;

    outputColor = vec4((ambient + diffuse + specular), 1.0f);
}